package net.thumbtack.school.elections.service;

import com.google.gson.Gson;
import net.thumbtack.school.elections.dao.*;
import net.thumbtack.school.elections.daoimpl.*;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.model.Candidate;
import net.thumbtack.school.elections.model.Offer;
import net.thumbtack.school.elections.model.Voter;
import net.thumbtack.school.elections.request.ListSomeoneDtoRequest;
import net.thumbtack.school.elections.request.TokenDtoRequest;
import net.thumbtack.school.elections.response.CandidatesDtoResponse;
import net.thumbtack.school.elections.response.ErrorDtoResponse;
import net.thumbtack.school.elections.response.OffersDtoResponse;
import net.thumbtack.school.elections.response.VotersDtoResponse;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

public class ListService {

    private static final double EPS = 1e-6;
    private static ListsDao listsDao = new ListsDaoImpl();
    private static OffersDao offersDao = new OffersDaoImpl();
    private static VoterAccessDao voterAccessDao = new VoterAccessDaoImpl();
    private Gson gson = new Gson();

    public String getVoters(String request) {
        try {
            TokenDtoRequest dtoRequest = gson.fromJson(request, TokenDtoRequest.class);
            dtoRequest.validate();

            if (!voterAccessDao.isValidToken(dtoRequest.getToken()))
                throw new ServerException(ServerExceptionCode.TOKEN_NOT_FOUND);

            List<Voter> voters = listsDao.getListVoters();

            List<VotersDtoResponse> response = new ArrayList<>(voters.size());
            for (Voter voter : voters)
                response.add(new VotersDtoResponse(voter));

            return gson.toJson(response);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String getOffers(String request) {
        try {
            TokenDtoRequest dtoRequest = gson.fromJson(request, TokenDtoRequest.class);
            dtoRequest.validate();

            if (!voterAccessDao.isValidToken(dtoRequest.getToken()))
                throw new ServerException(ServerExceptionCode.TOKEN_NOT_FOUND);

            List<Offer> offers = listsDao.getListOffers();

            List<OffersDtoResponse> response = new ArrayList<>(offers.size());
            for (Offer offer : offers)
                response.add(new OffersDtoResponse(offer));

            response.sort((o1, o2) -> {
                double avg1 = o1.getAvgScore();
                double avg2 = o2.getAvgScore();
                return Math.abs(avg1 - avg2) < EPS ? 0 : avg1 > avg2 ? -1 : 1;
            });

            return gson.toJson(response);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String getCandidates(String requestJsonString) {
        try {
            TokenDtoRequest dtoRequest = gson.fromJson(requestJsonString, TokenDtoRequest.class);
            dtoRequest.validate();

            if (!voterAccessDao.isValidToken(dtoRequest.getToken()))
                throw new ServerException(ServerExceptionCode.TOKEN_NOT_FOUND);

            Set<Candidate> candidates = listsDao.getListCandidates();

            List<CandidatesDtoResponse> response = new ArrayList<>();
            List<String> messages;
            for (Candidate candidate : candidates) {
                messages = new ArrayList<>();
                for (Offer offer : candidate.getProgramByCandidate()) {
                    messages.add(offer.getMessage());
                }
                response.add(new CandidatesDtoResponse(candidate.getName(), messages));
            }

            return gson.toJson(response);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String getOffersForSomeVoters(String requestJsonString) {
        try {
            ListSomeoneDtoRequest dtoRequest = gson.fromJson(requestJsonString, ListSomeoneDtoRequest.class);
            dtoRequest.validate();

            if (!voterAccessDao.isValidToken(dtoRequest.getToken()))
                throw new ServerException(ServerExceptionCode.TOKEN_NOT_FOUND);

            List<Integer> logins = dtoRequest.getIDs();
            List<CandidatesDtoResponse> response = new ArrayList<>();

            for (Integer ID : logins) {
                Voter voter = voterAccessDao.getVoterByID(ID);
                List<String> messages = new ArrayList<>();
                for (Offer offer : voter.getOffersByVoter()) {
                    messages.add(offer.getMessage());
                }
                response.add(new CandidatesDtoResponse(voter.getFullName(), messages));
            }

            return gson.toJson(response);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }
}
