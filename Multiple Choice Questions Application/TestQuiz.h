/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestQuiz.h
 * Author: Herman
 *
 * Created on May 11, 2023, 4:09:10 AM
 */

#ifndef TESTQUIZ_H
#define TESTQUIZ_H

#include <cppunit/extensions/HelperMacros.h>
#include "Quiz.h"

class TestQuiz : public CPPUNIT_NS::TestFixture {
    CPPUNIT_TEST_SUITE(TestQuiz);

    CPPUNIT_TEST(testAddQuestion);
    CPPUNIT_TEST(testRemoveQuestion);
    CPPUNIT_TEST(testMark);

    CPPUNIT_TEST_SUITE_END();

public:
    TestQuiz();
    virtual ~TestQuiz();
    void setUp();
    void tearDown();

private:
    void testAddQuestion();
    void testRemoveQuestion();
    void testMark();
};

#endif /* TESTQUIZ_H */

