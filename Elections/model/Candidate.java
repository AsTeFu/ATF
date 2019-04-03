package net.thumbtack.school.elections.model;

import net.thumbtack.school.elections.exception.ServerException;

import java.util.Set;

public class Candidate {

    private User user;
    private Program program;

    public Candidate(User user, Set<Offer> program) {
        this.user = user;
        this.program = new Program(program);
    }

    public User getUser() {
        return user;
    }
    public int getID() {
        return user.getID();
    }

    public void addOffer(Offer offer) throws ServerException {
        program.addOffer(offer);
    }

    public void removeOffer(Offer offer) throws ServerException {
        program.removeOffer(offer);
    }

    public void clearProgram() {
        program.clearProgram();
    }

    public Set<Offer> getProgramByCandidate() {
        return program.getAllOffers();
    }

    public String getName() {
        return user.getFullName();
    }
}
