# What is this?

A project seed for a C# dotnet API ("PaylocityBenefitsCalculator").  It is meant to get you started on the Paylocity BackEnd Coding Challenge by taking some initial setup decisions away.

The goal is to respect your time, avoid live coding, and get a sense for how you work.

# Coding Challenge

**Show us how you work.**

Each of our Paylocity product teams operates like a small startup, empowered to deliver business value in
whatever way they see fit. Because our teams are close knit and fast moving it is imperative that you are able
to work collaboratively with your fellow developers. 

This coding challenge is designed to allow you to demonstrate your abilities and discuss your approach to
design and implementation with your potential colleagues. You are free to use whatever technologies you
prefer but please be prepared to discuss the choices you’ve made. We encourage you to focus on creating a
logical and functional solution rather than one that is completely polished and ready for production.

The challenge can be used as a canvas to capture your strengths in addition to reflecting your overall coding
standards and approach. There’s no right or wrong answer.  It’s more about how you think through the
problem. We’re looking to see your skills in all three tiers so the solution can be used as a conversation piece
to show our teams your abilities across the board.

# Requirements

* Able to view employees and their dependents
* An employee may only have 1 spouse or domestic partner (not both)
* An employee may have an unlimited number of children
* Able to calculate and view a paycheck for an employee given the following rules:
    * 26 paychecks per year with deductions spread as evenly as possible on each paycheck
    * Employees have a base cost of $1,000 per month (for benefits)
    * Each dependent represents an additional $600 cost per month (for benefits)
    * Employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs
    * Dependents that are over 50 years old will incur an additional $200 per month
