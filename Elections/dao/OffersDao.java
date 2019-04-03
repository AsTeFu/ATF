package net.thumbtack.school.elections.dao;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.model.Offer;

public interface OffersDao {
    Offer getOffer(int ID) throws ServerException;
    int addOffer(Offer offer);
    boolean isValidID(int ID);
}
