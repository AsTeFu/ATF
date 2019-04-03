package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

public class AddOfferDtoRequest implements Validate {

    private String token;
    private String message;

    public AddOfferDtoRequest(String token, String message) {
        this.token = token;
        this.message = message;
    }

    public String getToken() {
        return token;
    }

    public String getMessage() {
        return message;
    }

    @Override
    public void validate() throws ServerException {
        if (isEmpty(token))
            throw new ServerException(ServerExceptionCode.NULL_TOKEN);

        if (isEmpty(message))
            throw new ServerException(ServerExceptionCode.NULL_MESSAGE);
    }
}
