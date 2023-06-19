/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestUser.cpp
 * Author: Herman
 *
 * Created on May 21, 2023, 4:57:35 PM
 */

#include "TestUser.h"


CPPUNIT_TEST_SUITE_REGISTRATION(TestUser);

TestUser::TestUser() {
}

TestUser::~TestUser() {
}

void TestUser::setUp() {
}

void TestUser::tearDown() {
}

void TestUser::testMethod() {
    CPPUNIT_ASSERT(true);
}

void TestUser::testFailedMethod() {
    CPPUNIT_ASSERT(false);
}

