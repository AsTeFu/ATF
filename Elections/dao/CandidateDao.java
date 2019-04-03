package net.thumbtack.school.elections.dao;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Candidate;

public interface CandidateDao {
    void add(Candidate candidate) throws ServerException;
    void addSomeone(Candidate candidate) throws ServerException;
    void agree(Candidate candidate) throws ServerException;
    void removeCandidate(Candidate candidate) throws ServerException;
    boolean isCandidate(int ID);
    Candidate getCandidate(int ID) throws ServerException;
    Candidate getPotentialCandidate(int ID) throws ServerException;
}
