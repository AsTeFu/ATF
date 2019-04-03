package net.thumbtack.school.elections.model;

import org.apache.commons.collections4.map.HashedMap;

import java.util.Map;

public class Offer {

    private static final Integer YOURSELF_SCORE = 5;
    private String message;
    private OfferAuthor author;
    private Map<OfferAuthor, Integer> score;
    // REVU интересная ситуация
    // с одной стороны, avgScore вычисляется через score
    // так что вроде как оно не нужно 
    // с другой стороны, Вы его вычисляете рекуррентно, и это очень хорошо
    // так как полной вычисление медленно
    // я думаю, разумное решение в том, чтобы создать class Score
    // в него и score, и avgScore
    // и методы addScore, removeScore и т.д.
    // тогда все действия со score будут инкапсулированы в нем
    // Offer не будет знать ничего про то, как там добавляется/удаляется, и это будет хорошо
    private double avgScore;

    public Offer(OfferAuthor author, String message) {
        this.author = author;
        this.message = message;

        score = new HashedMap<>();
        score.put(this.author, YOURSELF_SCORE);

        avgScore = YOURSELF_SCORE;
    }

    public String getMessage() {
        return message;
    }

    public OfferAuthor getAuthor() {
        return author;
    }

    public void setAuthor(OfferAuthor author) {
        this.author = author;
    }

    public void addScore(Voter voter, int value) {
        Integer oldValue = score.get(voter);

        score.put(voter, value);

        if (oldValue == null)
            avgScore = getAvgScore(value, 1);
        else avgScore = getAvgScore(value - oldValue, 0);

    }

    public void deleteScore(Voter voter) {
        avgScore = score.size() == 1 ? 0 : getAvgScore(-score.remove(voter), -1);
    }

    public boolean isLiked(Voter voter) {
        return score.containsKey(voter);
    }

    //offsetSize: при добавлении это 1, при удалении -1, при замене 0
    private double getAvgScore(int value, int offsetSize) {
        return (avgScore * (score.size() - offsetSize) + value) / score.size();
    }

    public double getAverageScore() {
        return avgScore;
    }

}
