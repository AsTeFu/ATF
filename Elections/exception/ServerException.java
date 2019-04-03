package net.thumbtack.school.elections.exception;

public class ServerException extends Exception {

    public ServerException(ServerExceptionCode errorCode) {
        super(errorCode.getMessageError());
    }

    public ServerException(ServerExceptionCode errorCode, int param1, int param2) {
        super(String.format(errorCode.getMessageError(), param1, param2));
    }

    public ServerException(ServerExceptionCode errorCode, String param) {
        super(String.format(errorCode.getMessageError(), param));
    }

}
