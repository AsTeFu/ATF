package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

public class IDOfferDtoRequest implements Validate {

    private String token;
    private int idOffer;
    private int idUser;

    public IDOfferDtoRequest(String token, int idUser, int idOffer) {
        this.token = token;
        this.idUser = idUser;
        this.idOffer = idOffer;
    }

    public String getToken() {
        return token;
    }
    public int getIdOffer() {
        return idOffer;
    }
    public int getIdUser() { return idUser; }

    @Override
    public void validate() throws ServerException {
        if (isEmpty(token))
            throw new ServerException(ServerExceptionCode.NULL_TOKEN);

        if (idOffer < 0)
            throw new ServerException(ServerExceptionCode.MESSAGE_NOT_FOUND, String.valueOf(idOffer));

    }

}
