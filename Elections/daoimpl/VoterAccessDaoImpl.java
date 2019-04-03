package net.thumbtack.school.elections.daoimpl;

import net.thumbtack.school.elections.dao.VoterAccessDao;
import net.thumbtack.school.elections.database.DataBase;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Voter;

public class VoterAccessDaoImpl implements VoterAccessDao {

    @Override
    public Voter getVoterByToken(String token) throws ServerException {
        return DataBase.getDataBase().getVoterByToken(token);
    }

    @Override
    public Voter getVoterByLogin(String login) throws ServerException {
        return DataBase.getDataBase().getVoterByLogin(login);
    }

    @Override
    public Voter getVoterByID(int ID) throws ServerException {
        return DataBase.getDataBase().getVoterByID(ID);
    }


    @Override
    public boolean isValidToken(String token) {
        return DataBase.getDataBase().isValidToken(token);
    }

}
