package net.thumbtack.school.elections.daoimpl;

import net.thumbtack.school.elections.dao.VoteDao;
import net.thumbtack.school.elections.database.DataBase;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.ItemBulletin;

import java.util.Map;

public class VoteDaoImpl implements VoteDao {

    @Override
    public void createBillyuteney(ItemBulletin... otherItemsBallot) {
        DataBase.getDataBase().createBulletin(otherItemsBallot);
    }

    @Override
    public void vote(int userID, int candidateID) throws ServerException {
        DataBase.getDataBase().vote(userID, candidateID);
    }

    @Override
    public Map<Integer, Integer> getListOfVotes() {
        return DataBase.getDataBase().getListOfVotes();
    }


}
