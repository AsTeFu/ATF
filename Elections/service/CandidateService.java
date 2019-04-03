package net.thumbtack.school.elections.service;

import java.util.Map;

import com.google.gson.Gson;
import net.thumbtack.school.elections.dao.CandidateDao;
import net.thumbtack.school.elections.dao.OffersDao;
import net.thumbtack.school.elections.dao.VoterAccessDao;
import net.thumbtack.school.elections.daoimpl.CandidateDaoImpl;
import net.thumbtack.school.elections.daoimpl.OffersDaoImpl;
import net.thumbtack.school.elections.daoimpl.VoterAccessDaoImpl;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.model.Candidate;
import net.thumbtack.school.elections.model.User;
import net.thumbtack.school.elections.model.Offer;
import net.thumbtack.school.elections.model.Voter;
import net.thumbtack.school.elections.request.IDOfferDtoRequest;
import net.thumbtack.school.elections.request.TokenIDDtoRequest;
import net.thumbtack.school.elections.request.TokenDtoRequest;
import net.thumbtack.school.elections.response.EmptyDtoResponse;
import net.thumbtack.school.elections.response.ErrorDtoResponse;

public class CandidateService {

    private static Gson gson = new Gson();

    private static VoterAccessDao voterAccessDao = new VoterAccessDaoImpl();
    private static CandidateDao candidateDao = new CandidateDaoImpl();
    private static OffersDao offersDao = new OffersDaoImpl();

    public String run(String requestJsonString) {
        try {
            TokenDtoRequest dtoRequest = gson.fromJson(requestJsonString, TokenDtoRequest.class);
            dtoRequest.validate();

            Voter voter = voterAccessDao.getVoterByToken(dtoRequest.getToken());
            Candidate candidate = new Candidate(voter.getUser(), voter.getOffersByVoter());

            candidateDao.add(candidate);

            return gson.toJson(new EmptyDtoResponse());

        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String runSomeone(String requestJsonString) {
        try {
            TokenIDDtoRequest dtoRequest = gson.fromJson(requestJsonString, TokenIDDtoRequest.class);
            dtoRequest.validate();

            if (!voterAccessDao.isValidToken(dtoRequest.getToken()))
                throw new ServerException(ServerExceptionCode.TOKEN_NOT_FOUND);

            Voter voter = voterAccessDao.getVoterByID(dtoRequest.getID());
            Candidate potentialCandidate = new Candidate(voter.getUser(), voter.getOffersByVoter());

            candidateDao.addSomeone(potentialCandidate);

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String agree(String requestJsonString) {
        try {
            TokenIDDtoRequest dtoRequest = gson.fromJson(requestJsonString, TokenIDDtoRequest.class);
            dtoRequest.validate();

            Candidate candidate = candidateDao.getPotentialCandidate(dtoRequest.getID());

            candidateDao.agree(candidate);

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String withdrawСandidacy(String requestJsonString) {
        try {
            TokenIDDtoRequest dtoRequest = gson.fromJson(requestJsonString, TokenIDDtoRequest.class);
            dtoRequest.validate();

            if (!voterAccessDao.isValidToken(dtoRequest.getToken()))
                throw new ServerException(ServerExceptionCode.TOKEN_NOT_FOUND);

            if (!candidateDao.isCandidate(dtoRequest.getID()))
                throw new ServerException(ServerExceptionCode.NOT_CANDIDATE, voterAccessDao.getVoterByID(dtoRequest.getID()).getFullName());

            Candidate candidate = candidateDao.getCandidate(dtoRequest.getID());

            candidate.clearProgram();
            candidateDao.removeCandidate(candidate);

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String addOffer(String requestJsonString) {
        try {
            IDOfferDtoRequest dtoRequest = gson.fromJson(requestJsonString, IDOfferDtoRequest.class);
            dtoRequest.validate();

            Candidate candidate = candidateDao.getCandidate(dtoRequest.getIdUser());

            int ID = dtoRequest.getIdOffer();
            if (!offersDao.isValidID(ID))
                throw new ServerException(ServerExceptionCode.MESSAGE_NOT_FOUND, String.valueOf(ID));

            Offer offer = offersDao.getOffer(ID);
            candidate.addOffer(offer);


            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String deleteOffer(String requestJsonString) {
        try {
            IDOfferDtoRequest dtoRequest = gson.fromJson(requestJsonString, IDOfferDtoRequest.class);
            dtoRequest.validate();

            Candidate candidate = candidateDao.getCandidate(dtoRequest.getIdUser());
            candidate.removeOffer(offersDao.getOffer(dtoRequest.getIdOffer()));

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }
}
