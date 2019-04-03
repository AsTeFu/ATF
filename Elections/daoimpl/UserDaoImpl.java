package net.thumbtack.school.elections.daoimpl;

import net.thumbtack.school.elections.dao.UserDao;
import net.thumbtack.school.elections.database.DataBase;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Voter;

public class UserDaoImpl implements UserDao {

    @Override
    public String authorize(Voter voter) {
        return DataBase.getDataBase().authorizeVoter(voter);
    }

    @Override
    public void logout(String token) throws ServerException {
        DataBase.getDataBase().logoutUser(token);
    }

    @Override
    public String register(Voter voter) throws ServerException {
        return DataBase.getDataBase().registerUser(voter);
    }

}
