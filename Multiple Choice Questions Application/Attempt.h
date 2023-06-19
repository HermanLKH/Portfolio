/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Attempt.h
 * Author: Herman
 *
 * Created on May 5, 2023, 10:34 PM
 */

#ifndef ATTEMPT_H
#define ATTEMPT_H
#pragma once
#include <string>
#include <vector>
#include <iostream>

using namespace std;

#include "Quiz.h"

class Attempt {
public:
    Attempt();
    Attempt(Quiz* q, int an);
    
    void DisplayReview();
    void DisplayReviewSummary();
    Quiz* GetQuiz();
    int GetScore();
    int GetTotalScore();
    void SetScores();
    
    Attempt(const Attempt& orig);
    virtual ~Attempt();
private:
    Quiz* quiz;
    vector<int> scores;
    int attemptNo;
    
};

#endif /* ATTEMPT_H */

