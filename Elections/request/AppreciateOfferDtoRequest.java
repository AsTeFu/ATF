package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

public class AppreciateOfferDtoRequest implements Validate {

    private static final int maxScore = 5;
    private static final int minScore = 1;

    private String token;
    private int ID;
    private int score;

    public AppreciateOfferDtoRequest(String token, int ID, int score) {
        this.token = token;
        this.ID = ID;
        this.score = score;
    }

    public String getToken() {
        return token;
    }

    public int getID() {
        return ID;
    }

    public int getScore() {
        return score;
    }

    @Override
    public void validate() throws ServerException {
        if (isEmpty(token))
            throw new ServerException(ServerExceptionCode.NULL_TOKEN);

        if (score > maxScore || score < minScore)
            throw new ServerException(ServerExceptionCode.INVALID_SCORE, minScore, maxScore);
    }
}
