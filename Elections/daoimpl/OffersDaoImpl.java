package net.thumbtack.school.elections.daoimpl;

import net.thumbtack.school.elections.dao.OffersDao;
import net.thumbtack.school.elections.database.DataBase;
import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Offer;

public class OffersDaoImpl implements OffersDao {

    @Override
    public Offer getOffer(int ID) throws ServerException {
        return DataBase.getDataBase().getOfferByID(ID);
    }

    @Override
    public boolean isValidID(int ID) {
        return DataBase.getDataBase().isValidID(ID);
    }

    @Override
    public int addOffer(Offer offer) {
        return DataBase.getDataBase().addOffer(offer);
    }

}
