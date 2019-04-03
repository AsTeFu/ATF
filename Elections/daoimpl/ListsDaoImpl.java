package net.thumbtack.school.elections.daoimpl;

import net.thumbtack.school.elections.dao.ListsDao;
import net.thumbtack.school.elections.database.DataBase;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;
import net.thumbtack.school.elections.model.Candidate;
import net.thumbtack.school.elections.model.Offer;
import net.thumbtack.school.elections.model.Voter;

import java.util.List;
import java.util.Set;

public class ListsDaoImpl implements ListsDao {
    @Override
    public Set<Candidate> getListCandidates() {
        return DataBase.getDataBase().getCandidates();
    }

    @Override
    public List<Offer> getListOffers() throws ServerException {
        return DataBase.getDataBase().getAllOffers();

    }
    
    @Override
    public List<Voter> getListVoters() throws ServerException {
        return DataBase.getDataBase().getAllVoters();
    }
}
