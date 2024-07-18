# Vehicle Auction TDD Kata

## Overview

This kata involves developing a vehicle auction system using Test-Driven Development (TDD). 
The goal is to create a system where vehicles can be auctioned, with features like setting a starting price, placing bids, maintaining bid history, setting reserve prices, and determining the winner of the auction.

## How to do it ?

Basically in TDD, there are 3 steps : **red** - **green** - **blue**.
- The **red** step consist of writing a test that fails.
- The **green** step consist of writing the minimum amount of code in the tested unit to make the test pass.
- The **blue** step consists of cleaning your code (generally at the end, in our case it's the last step after all the tests are done).

I recommend using this syntax for your tests : MethodName_ShouldDoSomething.
Since you will be testing more than one method, your tests will be easier to read.

## Phase 1 (Simple auction)

1. A vehicle auction should always have a vehicle, a starting price (decimal) and a reserve price (decimal). You want to be able to get those info from the vehicle auction.
   - FYI - Reserve price = The minimum acceptable item selling price.
2. You should be able to check the highest bid value, by default, this value should be the starting price.
3. Given a bidder's name and a value, you should be able to place a bid.
    - If the bid is higher than the current highest bid, then it becomes the new highest bid. 
    - Otherwise, it should throw a new InvalidOperationException with the message 'The bid amount must be higher than the current highest bid'.
4. You should be able to close the auction (we have a winner !)
    - The result of the close will be the highest amount.
    - The auction can only be won if the highest bid meets or exceeds the reserve price. If this condition is not respected, it should throw a new InvalidOperationException with the message 'The reserve price must be respected before closing the auction'.
5. Since you can now close the auction, you should not be able to bid when the auction is closed.
    - If you try to bid and the auction is closed, it should throw a new InvalidOperationException with the message 'You cannot bid when the auction is closed'.

## Phase 2 (Let's make this spicy)

1. I want to be able to consult the bid history. The first element will be the highest, in order.
2. Bidders can withdraw their bids unless the auction is closed. This will remove all bids from a certain bidder (using a bidder's name).
      - If the auction is closed, it should throw a new InvalidOperationException with the message 'You cannot withdraw bids when the auction is closed'.
3. Now, when the auction is closed, the result will be the amount and the bidder's name. 
      - If there is no bid in the auction, it should throw a new InvalidOperationException with the message 'There must be at least one bid to close the auction'.

## Phase 3 
This is what we call the blue phase. Clean up your code ;)