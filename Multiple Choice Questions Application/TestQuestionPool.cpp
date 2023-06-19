/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestQuestionPool.cpp
 * Author: Herman
 *
 * Created on May 11, 2023, 4:11:03 AM
 */

#include "TestQuestionPool.h"


CPPUNIT_TEST_SUITE_REGISTRATION(TestQuestionPool);

TestQuestionPool::TestQuestionPool() {
}

TestQuestionPool::~TestQuestionPool() {
}

void TestQuestionPool::setUp() {
}

void TestQuestionPool::tearDown() {
}

void TestQuestionPool::testAddQuestion() {
    QuestionPool* qp = new QuestionPool("QP 1");
    
    vector<Answer*> as = {
        new Answer(Answer::A, "Answer 1"),
        new Answer(Answer::B, "Answer 2"),
        new Answer(Answer::C, "Answer 3"),
        new Answer(Answer::D, "Answer 4")
    };
    
    Question* q1 = new Question("Question 1", as, 1);
    Question* q2 = new Question("Question 2", as, 2);
    
    CPPUNIT_ASSERT(qp->GetQuestions().size() == 0);
    
    qp->AddQuestion(q1);
    qp->AddQuestion(q2);
    CPPUNIT_ASSERT(qp->GetQuestions().size() == 2);
}

void TestQuestionPool::testRemoveQuestion() {
    QuestionPool* qp = new QuestionPool("QP 1");
    
    vector<Answer*> as = {
        new Answer(Answer::A, "Answer 1"),
        new Answer(Answer::B, "Answer 2"),
        new Answer(Answer::C, "Answer 3"),
        new Answer(Answer::D, "Answer 4")
    };
    
    Question* q1 = new Question("Question 1", as, 1);
    Question* q2 = new Question("Question 2", as, 2);
    qp->AddQuestion(q1);
    qp->AddQuestion(q2);
    CPPUNIT_ASSERT(qp->GetQuestions().size() == 2);
    
    qp->RemoveQuestion(1);
    qp->RemoveQuestion(0);
    CPPUNIT_ASSERT(qp->GetQuestions().size() == 0);
}

void TestQuestionPool::testSummary() {
    QuestionPool* qp = new QuestionPool("QP 1");
    
    vector<Answer*> as = {
        new Answer(Answer::A, "Answer 1"),
        new Answer(Answer::B, "Answer 2"),
        new Answer(Answer::C, "Answer 3"),
        new Answer(Answer::D, "Answer 4")
    };
    
    Question* q1 = new Question("Question 1", as, 1);
    Question* q2 = new Question("Question 2", as, 2);
    
    string expectedOutput = "QP 1, 0 questions\n";
    CPPUNIT_ASSERT(qp->Summary() == expectedOutput);
    
    qp->AddQuestion(q1);
    expectedOutput = "QP 1, 1 questions\n";
    CPPUNIT_ASSERT(qp->Summary() == expectedOutput);
}
