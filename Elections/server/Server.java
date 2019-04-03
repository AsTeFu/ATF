package net.thumbtack.school.elections.server;

import com.google.gson.Gson;
import net.thumbtack.school.elections.database.DataBase;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.response.EmptyDtoResponse;
import net.thumbtack.school.elections.response.ErrorDtoResponse;
import net.thumbtack.school.elections.service.*;

import java.io.*;

public class Server {

    private static Gson gson = new Gson();
    private boolean isLaunched;
    private boolean isVoteStarted;
    private VoterService voterService;
    private ListService listService;
    private OfferService offerService;
    private CandidateService candidateService;
    private VoteService voteService;

    public Server() {
        isLaunched = false;
        isVoteStarted = false;
    }

    public void startServer(String savedDataFileName) {
        voterService = new VoterService();
        listService = new ListService();
        offerService = new OfferService();
        candidateService = new CandidateService();
        voteService = new VoteService();

        if (savedDataFileName == null)
            DataBase.startDataBase();
        else {
            try (BufferedReader bufferedReader = new BufferedReader(new FileReader(savedDataFileName))) {
                DataBase.startDataBase(gson.fromJson(bufferedReader, DataBase.class));
            } catch (IOException ex) {
                //Файл не найден, сервер стартует с пустой базой данных
                DataBase.startDataBase();
            }
        }

        isLaunched = true;
    }

    public void stopServer(String saveDataFileName) {
        isLaunched = false;

        if (saveDataFileName != null) {
            try (BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(saveDataFileName))) {
                gson.toJson(DataBase.getDataBase(), bufferedWriter);
            } catch (IOException ex) {
                //База данных не была сохранена
            }
        }

    }

    private void validateServer() throws ServerException {
        if (!isLaunched)
            throw new ServerException(ServerExceptionCode.SERVER_NOT_LAUNCNED);
    }

    private void validateStartVoting(boolean vote) throws ServerException {
        if (vote)
            throw new ServerException(ServerExceptionCode.VOTING_IS_STARTED);
    }

    //Voters
    public String registerVoter(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return voterService.register(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String logoutVoter(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return voterService.logout(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String loginVoter(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return voterService.login(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    //Lists
    public String getListOfVoters(String requestJsonString) {
        try {
            validateServer();

            return listService.getVoters(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String getListOfOffers(String requestJsonString) {
        try {
            validateServer();

            return listService.getOffers(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String getListOfOffersForSomeVoters(String requestJsonString) {
        try {
            validateServer();

            return listService.getOffersForSomeVoters(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String getListOfCandidate(String requestJsonString) {
        try {
            validateServer();

            return listService.getCandidates(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    //Offers
    public String addOffer(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return offerService.addOffer(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String appreciateOffer(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return offerService.appreciateOffer(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String deleteScore(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return offerService.deleteScore(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    //Candidates
    public String runForElection(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return candidateService.run(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String runSomeoneForElection(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return candidateService.runSomeone(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String agree(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return candidateService.agree(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String withdrawСandidacy(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return candidateService.withdrawСandidacy(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String addOfferForProgram(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return candidateService.addOffer(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String deleteOfferFromProgram(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            return candidateService.deleteOffer(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    //Vote
    public String startVote() {
        try {
            validateServer();
            validateStartVoting(isVoteStarted);

            isVoteStarted = true;
            voteService.startVote();

            return gson.toJson(new EmptyDtoResponse());
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String stopVote() {
        try {
            validateServer();
            validateStartVoting(!isVoteStarted);

            isVoteStarted = false;

            return voteService.stopVote();
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

    public String vote(String requestJsonString) {
        try {
            validateServer();
            validateStartVoting(!isVoteStarted);

            return voteService.vote(requestJsonString);
        } catch (ServerException ex) {
            return gson.toJson(new ErrorDtoResponse(ex.getMessage()));
        }
    }

}
