package net.thumbtack.school.elections.response;

import java.util.List;

public class CandidatesDtoResponse {
    private String fullName;
    private List<String> messeges;

    public CandidatesDtoResponse(String fullName, List<String> messeges) {
        this.fullName = fullName;
        this.messeges = messeges;
    }
}


