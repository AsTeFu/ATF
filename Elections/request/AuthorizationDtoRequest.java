package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;

public class AuthorizationDtoRequest implements ValidateAuthorizationData {

    private String login;
    private String password;

    public String getLogin() {
        return login;
    }

    public String getPassword() {
        return password;
    }

    @Override
    public void validate() throws ServerException {
        checkLogin(login);
        checkPassword(password);
    }
}
