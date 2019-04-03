package net.thumbtack.school.elections.service;

import com.google.gson.Gson;
import net.thumbtack.school.elections.dao.*;
import net.thumbtack.school.elections.daoimpl.*;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.model.Society;
import net.thumbtack.school.elections.model.Voter;
import net.thumbtack.school.elections.request.AuthorizationDtoRequest;
import net.thumbtack.school.elections.request.RegisterVoterDtoRequest;
import net.thumbtack.school.elections.request.TokenDtoRequest;
import net.thumbtack.school.elections.response.EmptyDtoResponse;
import net.thumbtack.school.elections.response.ErrorDtoResponse;
import net.thumbtack.school.elections.response.TokenDtoResponse;

public class VoterService {

    private static final Society SOCIETY = new Society("Общество жителей города");

    private static Gson gson = new Gson();

    private static VoterAccessDao voterAccessDao = new VoterAccessDaoImpl();
    private static UserDao userDao = new UserDaoImpl();
    private static CandidateDao candidateDao = new CandidateDaoImpl();

    public String register(String requestJsonString) {
        try {
            RegisterVoterDtoRequest dtoRequest = gson.fromJson(requestJsonString, RegisterVoterDtoRequest.class);
            dtoRequest.validate();
            Voter voter = new Voter(dtoRequest.getFirstName(), dtoRequest.getLastName(), dtoRequest.getPatronymic(), dtoRequest.getAddress(),
                    dtoRequest.getBuilding(), dtoRequest.getApartment(), dtoRequest.getLogin(), dtoRequest.getPassword());

            String token = userDao.register(voter);
            return gson.toJson(new TokenDtoResponse(token));
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String login(String requestJsonString) {
        try {
            AuthorizationDtoRequest dtoRequest = gson.fromJson(requestJsonString, AuthorizationDtoRequest.class);
            dtoRequest.validate();
            Voter voter = voterAccessDao.getVoterByLogin(dtoRequest.getLogin());

            if (!voter.equalsPassword(dtoRequest.getPassword()))
                return gson.toJson(new ErrorDtoResponse(new ServerException(ServerExceptionCode.INVALID_PASSWORD).getMessage()));

            String token = userDao.authorize(voter);
            return gson.toJson(new TokenDtoResponse(token));
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String logout(String requestJsonString) {
        try {
            TokenDtoRequest dtoRequest = gson.fromJson(requestJsonString, TokenDtoRequest.class);
            dtoRequest.validate();

            Voter voter = voterAccessDao.getVoterByToken(dtoRequest.getToken());

            if (candidateDao.isCandidate(voter.getID()))
                throw new ServerException(ServerExceptionCode.HAS_PROGRAM);

            voter.clearLikedOffers();
            voter.clearOffers(SOCIETY);

            userDao.logout(dtoRequest.getToken());
            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }
}
