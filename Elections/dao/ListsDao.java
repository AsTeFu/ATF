package net.thumbtack.school.elections.dao;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Candidate;
import net.thumbtack.school.elections.model.Offer;
import net.thumbtack.school.elections.model.Voter;

import java.util.List;
import java.util.Set;

public interface ListsDao {
    Set<Candidate> getListCandidates();
    List<Offer> getListOffers() throws ServerException;
    List<Voter> getListVoters() throws ServerException;
}
