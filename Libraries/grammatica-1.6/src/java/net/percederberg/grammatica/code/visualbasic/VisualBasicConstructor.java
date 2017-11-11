/*
 * VisualBasicConstructor.java
 *
 * This program is free software: you can redistribute it and/or
 * modify it under the terms of the BSD license.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * LICENSE.txt file for more details.
 *
 * Copyright (c) 2004 Adrian Moore. All rights reserved.
 * Copyright (c) 2003-2015 Per Cederberg. All rights reserved.
 */

package net.percederberg.grammatica.code.visualbasic;

import java.io.PrintWriter;
import java.util.LinkedList;

import net.percederberg.grammatica.code.CodeElement;
import net.percederberg.grammatica.code.CodeStyle;

/**
 * A class generating a Visual Basic constructor declaration.
 *
 * @author   Adrian Moore
 * @author   Per Cederberg
 * @version  1.5
 * @since    1.5
 */
public class VisualBasicConstructor extends CodeElement {

    /**
     * The public access modifier constant.
     */
    public static final int PUBLIC = VisualBasicModifier.PUBLIC;

    /**
     * The protected friend access modifier constant.
     */
    public static final int PROTECTED_FRIEND =
        VisualBasicModifier.PROTECTED_FRIEND;

    /**
     * The protected access modifier constant.
     */
    public static final int PROTECTED = VisualBasicModifier.PROTECTED;

    /**
     * The friend access modifier constant.
     */
    public static final int FRIEND = VisualBasicModifier.FRIEND;

    /**
     * The private access modifier constant.
     */
    public static final int PRIVATE = VisualBasicModifier.PRIVATE;

    /**
     * The shared modifier constant.
     */
    public static final int SHARED = VisualBasicModifier.SHARED;

    /**
     * The modifier flags.
     */
    private int modifiers;

    /**
     * The class to construct.
     */
    private VisualBasicClass cls;

    /**
     * The argument list.
     */
    private String args;

    /**
     * The implementing code.
     */
    private LinkedList code;

    /**
     * The constructor comment.
     */
    private VisualBasicComment comment;

    /**
     * Creates a new empty constructor.
     */
    public VisualBasicConstructor() {
        this("");
    }

    /**
     * Creates a new constructor with the specified arguments.
     *
     * @param args           the argument list, excluding parenthesis
     */
    public VisualBasicConstructor(String args) {
        this(PUBLIC, args);
    }

    /**
     * Creates a new constructor with the specified arguments.
     *
     * @param modifiers      the modifier flags
     * @param args           the argument list, excluding parenthesis
     */
    public VisualBasicConstructor(int modifiers, String args) {
        this.modifiers = modifiers;
        this.cls = null;
        this.args = args;
        this.code = new LinkedList();
        this.comment = null;
    }

    /**
     * Returns the class for this constructor, or null.
     *
     * @return the class for this constructor, or
     *         null if none has been assigned
     */
    public VisualBasicClass getVisualBasicClass() {
        return this.cls;
    }

    /**
     * Sets the class for this constructor.
     *
     * @param cls      the class to add the constructor to
     */
    void setVisualBasicClass(VisualBasicClass cls) {
        this.cls = cls;
    }

    /**
     * Adds one or more lines of actual code.
     *
     * @param codeLines     the lines of Java code to add
     */
    public void addCode(String codeLines) {
        int  pos;

        pos = codeLines.indexOf('\n');
        while (pos >= 0) {
            this.code.add(codeLines.substring(0, pos));
            codeLines = codeLines.substring(pos + 1);
            pos = codeLines.indexOf('\n');
        }
        this.code.add(codeLines);
    }

    /**
     * Sets a comment for this constructor.
     *
     * @param comment       the new constructor comment
     */
    public void addComment(VisualBasicComment comment) {
        this.comment = comment;
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
        return 7;
    }

    /**
     * Prints the code element to the specified output stream.
     *
     * @param out            the output stream
     * @param style          the code style to use
     * @param indent         the indentation level
     */
    public void print(PrintWriter out, CodeStyle style, int indent) {
        String        indentStr = style.getIndent(indent);
        String        codeIndentStr = style.getIndent(indent + 1);
        StringBuffer  res = new StringBuffer();

        // Print comment
        if (comment != null) {
            comment.print(out, style, indent);
        }

        // Handle declaration
        res.append(indentStr);
        res.append(VisualBasicModifier.createModifierDecl(modifiers));
        res.append("Sub New(");
        res.append(args);
        res.append(")");

        // Handle code
        for (int i = 0; i < code.size(); i++) {
            if (code.get(i).toString().length() > 0) {
                res.append(codeIndentStr);
                res.append(code.get(i).toString());
                res.append("\n");
            } else {
                res.append("\n");
            }
        }
        res.append(indentStr);
        res.append("End Sub");

        // Print method
        out.println(res.toString());
    }
}
