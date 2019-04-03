package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

public class VoteDtoRequest implements Validate {

    private String token;
    private int candidateID;

    public VoteDtoRequest(String token, int candidateID) {
        this.token = token;
        this.candidateID = candidateID;
    }

    public String getToken() {
        return token;
    }

    public int getCandidateID() {
        return candidateID;
    }

    @Override
    public void validate() throws ServerException {
        if (isEmpty(token))
            throw new ServerException(ServerExceptionCode.NULL_TOKEN);
    }
}
