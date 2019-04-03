package net.thumbtack.school.elections.model;

import net.thumbtack.school.elections.exception.ServerException;
import net.thumbtack.school.elections.exception.ServerExceptionCode;

import java.util.HashSet;
import java.util.Set;

public class Program {
    private Set<Offer> offersByCandidate;
    private Set<Offer> offersByAnotherVoter;

    public Program(Set<Offer> offersByCandidate) {
        this.offersByCandidate = offersByCandidate;
        offersByAnotherVoter = new HashSet<>();
    }

    public void addOffer(Offer offer) throws ServerException {
        if (offersByCandidate.contains(offer) || !offersByAnotherVoter.add(offer))
            throw new ServerException(ServerExceptionCode.USED_MESSAGE);
    }

    public void removeOffer(Offer offer) throws ServerException {
        if (offersByCandidate.contains(offer))
            throw new ServerException(ServerExceptionCode.ACCESS_DENIED_MESSAGE);
        else if (!offersByAnotherVoter.remove(offer))
            throw new ServerException(ServerExceptionCode.NOT_USED_MESSAGE);
    }

    public void clearProgram() {
        offersByAnotherVoter.clear();
    }

    public Set<Offer> getAllOffers() {
        Set<Offer> tmp = new HashSet<>(offersByCandidate);
        tmp.addAll(offersByAnotherVoter);
        return tmp;
    }

}
