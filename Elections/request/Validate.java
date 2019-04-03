package net.thumbtack.school.elections.request;

import net.thumbtack.school.elections.exception.ServerException;

public interface Validate {

    void validate() throws ServerException;

    default boolean isEmpty(String str) {
        return str == null || str.trim().length() == 0;
    }

    default boolean isValidLength(String str, int minLen, int maxLen) {
        return str.length() >= minLen && str.length() <= maxLen;
    }

    default boolean containsSymbols(String str, int amount, String allowedSymbols) {
        int currentAmount = 0;

        for (char symbol : str.toCharArray())
            if (allowedSymbols.contains(String.valueOf(symbol)))
                if (++currentAmount == amount)
                    return true;

        return false;
    }

}
