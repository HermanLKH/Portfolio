/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestUser.h
 * Author: Herman
 *
 * Created on May 21, 2023, 4:57:34 PM
 */

#ifndef TESTUSER_H
#define TESTUSER_H

#include <cppunit/extensions/HelperMacros.h>

class TestUser : public CPPUNIT_NS::TestFixture {
    CPPUNIT_TEST_SUITE(TestUser);

    CPPUNIT_TEST(testMethod);
    CPPUNIT_TEST(testFailedMethod);

    CPPUNIT_TEST_SUITE_END();

public:
    TestUser();
    virtual ~TestUser();
    void setUp();
    void tearDown();

private:
    void testMethod();
    void testFailedMethod();
};

#endif /* TESTUSER_H */

