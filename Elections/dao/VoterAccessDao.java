package net.thumbtack.school.elections.dao;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Voter;

public interface VoterAccessDao {
    Voter getVoterByToken(String token) throws ServerException;
    Voter getVoterByLogin(String login) throws ServerException;
    Voter getVoterByID(int ID) throws ServerException;
    boolean isValidToken(String token);
}
