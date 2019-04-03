package net.thumbtack.school.elections.dao;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.ItemBulletin;

import java.util.Map;

public interface VoteDao {
    void createBillyuteney(ItemBulletin... itemsBallot);
    void vote(int userID, int candidateID) throws ServerException;
    Map<Integer, Integer> getListOfVotes();
}
