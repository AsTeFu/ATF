package net.thumbtack.school.elections.response;

public class MayorDtoResponse {
    private String newMayorName;
    private int amountVotes;

    public MayorDtoResponse(String newMayorName, int amountVotes) {
        this.newMayorName = newMayorName;
        this.amountVotes = amountVotes;
    }

    public String getNewMayorName() {
        return newMayorName;
    }

    public int getAmountVotes() {
        return amountVotes;
    }
}
