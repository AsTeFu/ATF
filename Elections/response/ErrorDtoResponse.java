package net.thumbtack.school.elections.response;

public class ErrorDtoResponse {

    private String error;

    public ErrorDtoResponse(String error) {
        this.error = error;
    }

    public String getError() {
        return error;
    }
}
