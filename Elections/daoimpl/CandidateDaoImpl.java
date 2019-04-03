package net.thumbtack.school.elections.daoimpl;

import net.thumbtack.school.elections.dao.CandidateDao;
import net.thumbtack.school.elections.database.DataBase;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Candidate;

public class CandidateDaoImpl implements CandidateDao {
    @Override
    public void add(Candidate candidate) throws ServerException {
        DataBase.getDataBase().addCandidate(candidate);
    }

    @Override
    public void addSomeone(Candidate candidate) throws ServerException {
        DataBase.getDataBase().addPotentialCandidate(candidate);
    }

    @Override
    public void agree(Candidate candidate) throws ServerException {
        DataBase.getDataBase().agreeCandidate(candidate);
    }

    @Override
    public void removeCandidate(Candidate candidate) throws ServerException {
        DataBase.getDataBase().removeCandidate(candidate);
    }

    @Override
    public boolean isCandidate(int ID) {
        return DataBase.getDataBase().isCandidate(ID);
    }

    @Override
    public Candidate getCandidate(int ID) throws ServerException {
        return DataBase.getDataBase().getCandidate(ID);
    }

    @Override
    public Candidate getPotentialCandidate(int ID) throws ServerException {
        return DataBase.getDataBase().getPotentialCandidate(ID);
    }

}
