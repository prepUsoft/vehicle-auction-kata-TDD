# Car Auction TDD Kata

## Overview

This kata involves developing a car auction system using Test-Driven Development (TDD). The goal is to create a system where cars can be auctioned, with features like setting a starting price, placing bids, maintaining bid history, setting reserve prices, and determining the winner of the auction.

## Requirements

1. A car should have a starting price and a reserve price.
2. The highest bid should initially be the starting price.
3. Bidders can place bids higher than the current highest bid.
4. The highest bid should be updated with each valid bid.
5. Maintain a bid history for each car.
6. The auction can be closed.
7. The auction can only be won if the highest bid meets or exceeds the reserve price.
8. Bidders can withdraw their bids unless the auction is closed.
9. Track the highest bidder. Test2