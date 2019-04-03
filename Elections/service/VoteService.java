package net.thumbtack.school.elections.service;

import com.google.gson.Gson;
import net.thumbtack.school.elections.dao.CandidateDao;
import net.thumbtack.school.elections.dao.VoteDao;
import net.thumbtack.school.elections.dao.VoterAccessDao;
import net.thumbtack.school.elections.daoimpl.CandidateDaoImpl;
import net.thumbtack.school.elections.daoimpl.VoteDaoImpl;
import net.thumbtack.school.elections.daoimpl.VoterAccessDaoImpl;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.model.*;
import net.thumbtack.school.elections.request.VoteDtoRequest;
import net.thumbtack.school.elections.response.EmptyDtoResponse;
import net.thumbtack.school.elections.response.ErrorDtoResponse;
import net.thumbtack.school.elections.response.MayorDtoResponse;

import java.util.Map;

public class VoteService {

    private static final ItemBulletin againstAll = new ItemBulletin("Против всех");
    private static Gson gson = new Gson();
    private static VoteDao voteDao = new VoteDaoImpl();
    private static VoterAccessDao voterAccessDao = new VoterAccessDaoImpl();
    private static CandidateDao candidateDao = new CandidateDaoImpl();

    public void startVote() {
        voteDao.createBillyuteney(againstAll);
    }


    public String vote(String request) {
        try {
            VoteDtoRequest dtoRequest = gson.fromJson(request, VoteDtoRequest.class);
            dtoRequest.validate();

            Voter voter = voterAccessDao.getVoterByToken(dtoRequest.getToken());

            if (voter.getID() == dtoRequest.getCandidateID())
                throw new ServerException(ServerExceptionCode.VOTED_FOR_HIMSELF);

            voteDao.vote(voter.getID(), dtoRequest.getCandidateID());

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String stopVote() {
        try {
            Map<Integer, Integer> vote = voteDao.getListOfVotes();
            int amountAgainstAll = vote.remove(againstAll.getID());
            int maxAmoutVotes = 0;

            if (vote.size() == 0)
                throw new ServerException(ServerExceptionCode.WITHOUT_CANDIDATES);

            Candidate mayor = null;
            for (Integer ID : vote.keySet()) {
                if (vote.get(ID) > maxAmoutVotes) {
                    maxAmoutVotes = vote.get(ID);
                    mayor = candidateDao.getCandidate(ID);
                }
            }

            if (maxAmoutVotes < amountAgainstAll)
                throw new ServerException(ServerExceptionCode.ELECTIONS_FAILED);

            MayorDtoResponse response = new MayorDtoResponse(mayor.getName(), maxAmoutVotes);
            return gson.toJson(response);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

}
