package net.thumbtack.school.elections.service;

import com.google.gson.Gson;
import net.thumbtack.school.elections.dao.OffersDao;
import net.thumbtack.school.elections.dao.VoterAccessDao;
import net.thumbtack.school.elections.daoimpl.OffersDaoImpl;
import net.thumbtack.school.elections.daoimpl.VoterAccessDaoImpl;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.model.Offer;
import net.thumbtack.school.elections.model.Voter;
import net.thumbtack.school.elections.request.AddOfferDtoRequest;
import net.thumbtack.school.elections.request.AppreciateOfferDtoRequest;
import net.thumbtack.school.elections.request.IDOfferDtoRequest;
import net.thumbtack.school.elections.response.AddOfferResponse;
import net.thumbtack.school.elections.response.EmptyDtoResponse;
import net.thumbtack.school.elections.response.ErrorDtoResponse;

public class OfferService {

    private static Gson gson = new Gson();

    private OffersDao offersDao = new OffersDaoImpl();
    private VoterAccessDao voterAccessDao = new VoterAccessDaoImpl();

    public String addOffer(String request) {
        try {
            AddOfferDtoRequest dtoRequest = gson.fromJson(request, AddOfferDtoRequest.class);
            dtoRequest.validate();

            Voter author = voterAccessDao.getVoterByToken(dtoRequest.getToken());
            Offer offer = new Offer(author, dtoRequest.getMessage());
            int ID = offersDao.addOffer(offer);

            //Liked offer
            author.addLikedOffer(offer);
            //Offer
            author.addOffer(offer);

            return gson.toJson(new AddOfferResponse(ID));
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String appreciateOffer(String request) {
        try {
            AppreciateOfferDtoRequest dtoRequest = gson.fromJson(request, AppreciateOfferDtoRequest.class);
            dtoRequest.validate();

            Voter voter = voterAccessDao.getVoterByToken(dtoRequest.getToken());
            Offer offer = offersDao.getOffer(dtoRequest.getID());

            if (voter.getOffersByVoter().contains(offer))
                throw new ServerException(ServerExceptionCode.ACCESS_DENIED_SCORE);

            offer.addScore(voter, dtoRequest.getScore());

            //Liked offer
            voter.addLikedOffer(offer);

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String deleteScore(String request) {
        try {
            IDOfferDtoRequest dtoRequest = gson.fromJson(request, IDOfferDtoRequest.class);
            dtoRequest.validate();

            Offer offer = offersDao.getOffer(dtoRequest.getIdOffer());
            Voter voter = voterAccessDao.getVoterByToken(dtoRequest.getToken());

            //Попытка удалить оценку себе или несуществующую оценку
            if (offer.getAuthor().equals(voter) || !offer.isLiked(voter))
                throw new ServerException(ServerExceptionCode.ACCESS_DENIED_SCORE);

            offer.deleteScore(voter);

            //Liked offer
            voter.removeLikedOffer(offer);

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

}
