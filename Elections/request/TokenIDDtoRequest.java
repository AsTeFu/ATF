package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

public class TokenIDDtoRequest implements Validate {

    private String token;
    private int ID;

    public TokenIDDtoRequest(String token, int ID) {
        this.token = token;
        this.ID = ID;
    }

    public String getToken() {
        return token;
    }

    public int getID() {
        return ID;
    }

    @Override
    public void validate() throws ServerException {
        if (isEmpty(token))
            throw new ServerException(ServerExceptionCode.NULL_TOKEN);
    }
}
