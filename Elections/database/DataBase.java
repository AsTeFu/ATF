package net.thumbtack.school.elections.database;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.model.Candidate;
import net.thumbtack.school.elections.model.ItemBulletin;
import net.thumbtack.school.elections.model.Offer;
import net.thumbtack.school.elections.model.Voter;
import org.apache.commons.collections4.map.HashedMap;

import java.util.*;

public class DataBase {
    private static DataBase instance;
    private static final GeneratorID generatorIdForOffers = new GeneratorID(0);

    private Map<String, Voter> usersByLogin;
    private Map<String, Voter> authorizedUsers;
    private Map<Integer, Voter> usersByID;

    private List<Offer> offers;

    private Map<Integer, Candidate> candidates;
    private Map<Integer, Candidate> potentialCandidates;

    private Map<Integer, Integer> listOfVotes;
    private Set<Integer> alreadyVoted;

    public DataBase() {
        usersByLogin = new HashedMap<>();
        authorizedUsers = new HashedMap<>();
        offers = new ArrayList<>();
        candidates = new HashMap<>();
        potentialCandidates = new HashMap<>();
        listOfVotes = new HashMap<>();
        alreadyVoted = new HashSet<>();
        usersByID = new HashMap<>();
    }

    public static void startDataBase() {
        if (instance == null)
            instance = new DataBase();
    }
    public static void startDataBase(DataBase dataBase) {
        if (instance == null)
            instance = dataBase;
    }
    public static DataBase getDataBase() {
        if (instance == null)
            instance = new DataBase();
        return instance;
    }

    //Voter
    public String registerUser(Voter voter) throws ServerException {
        if (usersByLogin.containsKey(voter.getLogin()))
            throw new ServerException(ServerExceptionCode.USED_LOGIN, voter.getLogin());

        usersByLogin.put(voter.getLogin(), voter);
        usersByID.put(voter.getID(), voter);

        return authorizeVoter(voter);
    }
    public String authorizeVoter(Voter voter) {
        String token = createNewToken();
        authorizedUsers.put(token, voter);

        return token;
    }
    public Voter getVoterByLogin(String login) throws ServerException {
        return getVoter(login, usersByLogin, ServerExceptionCode.USER_NOT_FOUND);
    }
    public Voter getVoterByToken(String token) throws ServerException {
        return getVoter(token, authorizedUsers, ServerExceptionCode.TOKEN_NOT_FOUND);
    }
    public Voter getVoterByID(int ID) throws ServerException {
    	// REVU не надо вызывать containsKey. Вызовите get и проверьте результат, и будет 1 запрос к мэпу, а не 2
        if (!usersByID.containsKey(ID))
            throw new ServerException(ServerExceptionCode.INVALID_ID);

        return usersByID.get(ID);
    }
    private Voter getVoter(String key, Map<String, Voter> collection, ServerExceptionCode errorCode) throws ServerException {
    	// REVU то же самое
        if (!collection.containsKey(key))
            throw new ServerException(errorCode);

        return collection.get(key);
    }
    public List<Voter> getAllVoters() {
        return new ArrayList<>(usersByLogin.values());
    }

    //Token
    public void logoutUser(String token) throws ServerException {
        if (authorizedUsers.remove(token) == null)
            throw new ServerException(ServerExceptionCode.TOKEN_NOT_FOUND);
    }
    public boolean isValidToken(String token) {
        return authorizedUsers.containsKey(token);
    }
    private String createNewToken() {
    	// REVU зачем минусы удалять ? И так сойдет
        return UUID.randomUUID().toString().replace("-", "");
    }

    //Offer
    public int addOffer(Offer offer) {
        offers.add(offer);
        return generatorIdForOffers.generateNext();
    }
    public Offer getOfferByID(int ID) throws ServerException {
        if (ID < 0 || ID >= offers.size())
            throw new ServerException(ServerExceptionCode.MESSAGE_NOT_FOUND, String.valueOf(ID));

        return offers.get(ID);
    }
    public List<Offer> getAllOffers() {
        return offers;
    }
    public boolean isValidID(int ID) {
        return ID >= 0 && ID < offers.size();
    }

    //Candidate
    public Candidate getCandidate(int ID) throws ServerException {
        if (!candidates.containsKey(ID))
            throw new ServerException(ServerExceptionCode.INVALID_ID);

        return candidates.get(ID);
    }
    public Candidate getPotentialCandidate(int ID) throws ServerException {
        if (!potentialCandidates.containsKey(ID))
            throw new ServerException(ServerExceptionCode.INVALID_ID);

        return potentialCandidates.get(ID);
    }
    public void addCandidate(Candidate candidate) throws ServerException {
        if (candidates.containsKey(candidate.getID()))
            throw new ServerException(ServerExceptionCode.ALREADY_CANDIDATE, candidate.getName());

        candidates.put(candidate.getID(), candidate);
    }
    public void addPotentialCandidate(Candidate candidate) throws ServerException {
        if (potentialCandidates.containsKey(candidate.getID()))
            throw new ServerException(ServerExceptionCode.ALREADY_POTENTIAL_CANDIDATE, candidate.getName());
        if (candidates.containsKey(candidate.getID()))
            throw new ServerException(ServerExceptionCode.ALREADY_CANDIDATE, candidate.getName());

        potentialCandidates.put(candidate.getID(), candidate);
    }
    public void agreeCandidate(Candidate candidate) throws ServerException {
        if (potentialCandidates.remove(candidate.getID()) == null)
            throw new ServerException(ServerExceptionCode.NOT_POTENTIAL_CANDIDATE);

        addCandidate(candidate);
    }
    public void removeCandidate(Candidate candidate) throws ServerException {
        if (candidates.remove(candidate.getID()) == null)
            throw new ServerException(ServerExceptionCode.NOT_CANDIDATE, candidate.getName());
    }
    public Set<Candidate> getCandidates() {
        return new HashSet<>(candidates.values());
    }
    public boolean isCandidate(int ID) {
        return candidates.containsKey(ID);
    }

    //Vote
    //Убирает из списка кандидатов тех, у кого нет программы на начало голосования
    //otherItemBallot: другие элементы голосования, такие как "против всех"
    public void createBulletin(ItemBulletin... otherItemsBallot) {
        listOfVotes = new HashMap<>();

        for (Integer ID : candidates.keySet())
            if (candidates.get(ID).getProgramByCandidate().size() > 0)
                listOfVotes.put(ID, 0);

        int indexOtherItems = usersByID.size();
        for (ItemBulletin item : otherItemsBallot) {
            item.setID(++indexOtherItems);
            listOfVotes.put(item.getID(), 0);
        }
    }
    public void vote(int userID, int candidateID) throws ServerException {
        if (!listOfVotes.containsKey(candidateID))
            throw new ServerException(ServerExceptionCode.NOT_CANDIDATE, usersByID.get(candidateID).getFullName());

        if (!alreadyVoted.add(userID))
            throw new ServerException(ServerExceptionCode.ALREADY_VOTED);

        listOfVotes.replace(candidateID, listOfVotes.get(candidateID) + 1);
    }
    public Map<Integer, Integer> getListOfVotes() {
        return listOfVotes;
    }

}