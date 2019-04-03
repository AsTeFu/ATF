package net.thumbtack.school.elections.model;

import java.util.Objects;

public class Society implements OfferAuthor {

    private String societyName;

    public Society(String societyName) {
        this.societyName = societyName;
    }


    @Override
    public String getFullName() {
        return societyName;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Society society = (Society) o;
        return Objects.equals(societyName, society.societyName);
    }

    @Override
    public int hashCode() {
        return Objects.hash(societyName);
    }
}
