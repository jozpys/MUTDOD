/*
 * JavaInterface.java
 *
 * This program is free software: you can redistribute it and/or
 * modify it under the terms of the BSD license.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * LICENSE.txt file for more details.
 *
 * Copyright (c) 2003-2015 Per Cederberg. All rights reserved.
 */

package net.percederberg.grammatica.code.java;

import java.io.PrintWriter;

import net.percederberg.grammatica.code.CodeStyle;

/**
 * A class generating a Java interface declaration.
 *
 * @author   Per Cederberg
 * @version  1.0
 */
public class JavaInterface extends JavaType {

    /**
     * The public access modifier constant.
     */
    public static final int PUBLIC = JavaModifier.PUBLIC;

    /**
     * The protected access modifier constant. May only be used when
     * declared inside a class.
     */
    public static final int PROTECTED = JavaModifier.PROTECTED;

    /**
     * The package local access modifier constant (i.e. no modifier).
     */
    public static final int PACKAGE_LOCAL = JavaModifier.PACKAGE_LOCAL;

    /**
     * The private access modifier constant. May only be used when
     * declared inside a class.
     */
    public static final int PRIVATE = JavaModifier.PRIVATE;

    /**
     * The static modifier constant. May only be used when declared
     * inside a class.
     */
    public static final int STATIC = JavaModifier.STATIC;

    /**
     * The strictfp modifier constant.
     */
    public static final int STRICTFP = JavaModifier.STRICTFP;

    /**
     * Creates a new interface code generator with a public access
     * modifier.
     *
     * @param name           the class name
     */
    public JavaInterface(String name) {
        this(PUBLIC, name);
    }

    /**
     * Creates a new interface code generator with the specified
     * access modifier.
     *
     * @param modifiers      the modifier constant flags
     * @param name           the class name
     */
    public JavaInterface(int modifiers, String name) {
        this(modifiers, name, "");
    }

    /**
     * Creates a new class code generator with the specified access
     * modifier that extends the specified class.
     *
     * @param modifiers      the modifier constant flags
     * @param name           the class name
     * @param extendType     the type to extend
     */
    public JavaInterface(int modifiers, String name, String extendType) {
        super(modifiers, name, extendType, "");
    }

    /**
     * Adds a method declaration to the interface.
     *
     * @param member         the member to add
     */
    public void addMethod(JavaMethod member) {
        member.setPrintCode(false);
        addElement(member);
    }

    /**
     * Adds a variable to the interface.
     *
     * @param member         the member to add
     */
    public void addVariable(JavaVariable member) {
        addElement(member);
    }

    /**
     * Returns a numeric category number for the code element. A lower
     * category number implies that the code element should be placed
     * before code elements with a higher category number within a
     * declaration.
     *
     * @return the category number
     */
    public int category() {
        return 9;
    }

    /**
     * Prints the code element to the specified output stream.
     *
     * @param out            the output stream
     * @param style          the code style to use
     * @param indent         the indentation level
     */
    public void print(PrintWriter out, CodeStyle style, int indent) {
        super.print(out, style, indent, "interface");
    }
}
