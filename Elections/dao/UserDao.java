package net.thumbtack.school.elections.dao;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Voter;

public interface UserDao {
    String authorize(Voter login);
    String register(Voter voter) throws ServerException;
    void logout(String token) throws ServerException;
}
