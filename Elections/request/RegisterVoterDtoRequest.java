package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

public class RegisterVoterDtoRequest implements ValidateAuthorizationData {


    private String firstName;
    private String lastName;
    private String patronymic;

    private String address;
    private String building;
    private int apartment;

    private String login;
    private String password;

    public RegisterVoterDtoRequest(String firstName, String lastName, String patronymic, String address, String building, int apartment, String login, String password) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.patronymic = patronymic;
        this.address = address;
        this.building = building;
        this.apartment = apartment;
        this.login = login;
        this.password = password;
    }

    public String getFirstName() {
        return firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public String getPatronymic() {
        return patronymic;
    }

    public String getAddress() {
        return address;
    }

    public String getBuilding() {
        return building;
    }

    public int getApartment() {
        return apartment;
    }

    public String getLogin() {
        return login;
    }

    public String getPassword() {
        return password;
    }

    @Override
    public void validate() throws ServerException {
        checkPassword(password);
        checkLogin(login);

        checkField(firstName, ServerExceptionCode.WRONG_FIRSTNAME);
        checkField(lastName, ServerExceptionCode.WRONG_LASTNAME);
        checkField(address, ServerExceptionCode.WRONG_ADDRESS);
        checkField(building, ServerExceptionCode.WRONG_BUILDING);
    }

    private void checkField(String value, ServerExceptionCode errorCode) throws ServerException {
        if (isEmpty(value))
            throw new ServerException(errorCode);
    }

}
