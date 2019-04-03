package net.thumbtack.school.elections.response;

import net.thumbtack.school.elections.model.Voter;

public class VotersDtoResponse {

    private String firstName;
    private String lastName;
    private String patronymic;

    public VotersDtoResponse(String firstName, String lastName, String patronymic) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.patronymic = patronymic;
    }

    public VotersDtoResponse(Voter voter) {
        this(voter.getUser().getFirstName(), voter.getUser().getLastName(), voter.getUser().getPatronymic());
    }

}
