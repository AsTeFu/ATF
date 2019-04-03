package net.thumbtack.school.elections.model;

import java.util.HashSet;
import java.util.Objects;
import java.util.Set;

public class Voter implements OfferAuthor {

    private User user;

    private Set<Offer> likedOffers;
    private Set<Offer> offersByVoter;

    private Voter() {
        likedOffers = new HashSet<>();
        offersByVoter = new HashSet<>();
    }

    public Voter(String firstName, String lastName, String patronymic, String address, String building, int apartment, String login, String password) {
        this();
        user = new User(firstName, lastName, patronymic, address, building, apartment, login, password);
    }

    public User getUser() {
        return user;
    }
    public String getLogin() {
        return user.getLogin();
    }
    public int getID() {
        return user.getID();
    }

    //Liked offers
    public void addLikedOffer(Offer offer) {
        likedOffers.add(offer);
    }
    public void removeLikedOffer(Offer offer) {
        likedOffers.remove(offer);
    }
    public void clearLikedOffers() {
        for (Offer offer : likedOffers)
            offer.deleteScore(this);

        likedOffers.clear();
    }

    public void addOffer(Offer offer) {
        offersByVoter.add(offer);
    }
    public void clearOffers(OfferAuthor newAuthor) {
        for (Offer offer : offersByVoter)
            offer.setAuthor(newAuthor);

        offersByVoter.clear();
    }
    public Set<Offer> getOffersByVoter() {
        return offersByVoter;
    }

    @Override
    public String getFullName() {
        return user.getFullName();
    }
    public boolean equalsPassword(String password) {
        return this.user.getPassword().equals(password);
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Voter voter = (Voter) o;
        return user.equals(voter.user);
    }
    @Override
    public int hashCode() {
        return Objects.hash(user);
    }
}
