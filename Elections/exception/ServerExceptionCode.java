package net.thumbtack.school.elections.exception;

public enum ServerExceptionCode {
    WRONG_PASSWORD_LEN("Пароль должен состоять из %d-%d символов"),
    WRONG_PASSWORD("Пароль должен содержать: цифр: %d и cпец. символов: %d"),
    NULL_PASSWORD("Пароль не задан"),
    INVALID_PASSWORD("Неверный пароль"),

    NULL_LOGIN("Логин не задан"),
    WRONG_LOGIN_LEN("Логин должен состоять из %d-%d символов"),
    USED_LOGIN("Логин %s уже занят"),

    WRONG_FIRSTNAME("Некорректное имя"),
    WRONG_LASTNAME("Некорректная фамилия"),
    WRONG_ADDRESS("Некорректный адресс"),
    WRONG_BUILDING("Некорректный номер дома"),

    NULL_TOKEN("Пустой токен"),
    TOKEN_NOT_FOUND("Некорректный токен"),

    USER_NOT_FOUND("Пользователь не найден"),
    INVALID_ID("Неправильный ID"),

    NULL_MESSAGE("Пустое сообщение"),
    MESSAGE_NOT_FOUND("Сообщение с ID %s не найдено"),
    INVALID_SCORE("Оценка должна находиться в границах [%d, %d]"),
    ACCESS_DENIED_SCORE("Нелья менять оценку своему предложению"),
    USED_MESSAGE("Это предложение уже есть в программе"),
    NOT_USED_MESSAGE("Это предложение не найдненно в программе"),
    ACCESS_DENIED_MESSAGE("Нельзя удалять свое предложение из программы"),



    ALREADY_CANDIDATE("%s уже является кандидатом"),
    ALREADY_POTENTIAL_CANDIDATE("%s уже был выдвинут, но все еще не дал согласия"),
    NOT_POTENTIAL_CANDIDATE("Вас никто не выдвигал"),
    NOT_CANDIDATE("%s не является кандидатом"),
    HAS_PROGRAM("Прежде, чем покинуть сервер, нужно снять кандидатуру"),

    SERVER_NOT_LAUNCNED("Cервер не запущен"),
    VOTING_IS_STARTED("Голосование уже началось"),

    ALREADY_VOTED("Голосовать можно только один раз"),
    VOTED_FOR_HIMSELF("Нельзя голосовать за себя"),
    WITHOUT_CANDIDATES("На выборах не было ни одного кандидата"),
    ELECTIONS_FAILED("Выборы не состоялись");

    private String message;

    ServerExceptionCode(String message) {
        this.message = message;
    }

    public String getMessageError() {
        return message;
    }
}
