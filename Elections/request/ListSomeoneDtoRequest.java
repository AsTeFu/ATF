package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

import java.util.List;

public class ListSomeoneDtoRequest implements Validate {

    private String token;
    private List<Integer> IDs;

    public ListSomeoneDtoRequest(String token, List<Integer> IDs) {
        this.token = token;
        this.IDs = IDs;
    }

    public String getToken() {
        return token;
    }

    public List<Integer> getIDs() {
        return IDs;
    }

    @Override
    public void validate() throws ServerException {
        if (isEmpty(token))
            throw new ServerException(ServerExceptionCode.NULL_TOKEN);
    }
}
