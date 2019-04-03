package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

public interface ValidateAuthorizationData extends Validate {

    int MIN_PASSWORD_LEN = 8;
    int MAX_PASSWORD_LEN = 40;
    int MIN_LOGIN_LEN = 5;
    int MAX_LOGIN_LEN = 20;

    int MIN_AMOUNT_NUMBERS = 2;
    int MIN_AMOUNT_SPECIAL_SYMBOLS = 1;

    String ALL_DIGIT = "1234567890";
    String ALL_SPEC_SYMBOL = "!@#$%^&*()-_+=;:,./?\\|`~[]{}";

    default void checkPassword(String password) throws ServerException {
        if (isEmpty(password))
            throw new ServerException(ServerExceptionCode.NULL_PASSWORD);

        if (!isValidLength(password, MIN_PASSWORD_LEN, MAX_PASSWORD_LEN))
            throw new ServerException(ServerExceptionCode.WRONG_PASSWORD_LEN, MIN_PASSWORD_LEN, MAX_PASSWORD_LEN);

        if (!containsSymbols(password, MIN_AMOUNT_NUMBERS, ALL_DIGIT) ||
                !containsSymbols(password, MIN_AMOUNT_SPECIAL_SYMBOLS, ALL_SPEC_SYMBOL))
            throw new ServerException(ServerExceptionCode.WRONG_PASSWORD, MIN_AMOUNT_NUMBERS, MIN_AMOUNT_SPECIAL_SYMBOLS);
    }

    default void checkLogin(String login) throws ServerException {
        if (isEmpty(login))
            throw new ServerException(ServerExceptionCode.NULL_LOGIN);

        if (!isValidLength(login, MIN_LOGIN_LEN, MAX_LOGIN_LEN))
            throw new ServerException(ServerExceptionCode.WRONG_LOGIN_LEN, MIN_LOGIN_LEN, MAX_LOGIN_LEN);
    }

}
