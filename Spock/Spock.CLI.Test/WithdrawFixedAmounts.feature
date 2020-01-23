
Feature: Withdraw Fixed Amounts

Scenarios: Withdraw fixed amount
  Given I have <Balance> in my account
  When I choose to withdraw a fixed amount of <Withdrawal>
  Then I should <Outcome>
    And the balance of my account should be <Remaining>

  Where: Successful withdrawal
    Can add an optional description in addition to the name for an Examples, Where keyword.
    | Balance | Withdrawal | Outcome           | Remaining |
    | 500D    | 50D        | receive $50 cash  | 450D      |
    | 500     | 100        | receive $100 cash | 400       |

  Where: Attempt to withdraw too much
    | Balance | Withdrawal | Outcome       | Remaining |
    | 100D    | 200D       | see an error  | 100D      |
    | 0       |  50        | see an error  | 0         |