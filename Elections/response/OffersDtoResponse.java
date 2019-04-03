package net.thumbtack.school.elections.response;

import net.thumbtack.school.elections.model.Offer;

public class OffersDtoResponse {

    private String authorName;
    private String message;
    private double avgScore;

    public OffersDtoResponse(String authorName, String message, double avgScore) {
        this.authorName = authorName;
        this.message = message;
        this.avgScore = avgScore;
    }

    public OffersDtoResponse(Offer offer) {
        this(offer.getAuthor().getFullName(), offer.getMessage(), offer.getAverageScore());
    }

    public double getAvgScore() {
        return avgScore;
    }
}
