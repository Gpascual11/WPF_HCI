﻿// RelayCommand.cs
// ----------------------------------------------------------------------
// This file defines the RelayCommand class, which is a simple implementation 
// of the ICommand interface. RelayCommand allows you to pass an Action to be
// executed and an optional predicate to determine whether the command can 
// execute. This class is commonly used in MVVM to bind commands from the 
// ViewModel to the View.
// 
// Author: Gerard Pascual
// Date: 4/4/2025
// Version: 1.1
// ----------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace WPF_HCI
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects.
    /// The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        // The action to execute.
        private readonly Action execute;
        // The predicate to determine if the command can execute.
        private readonly Func<bool>? canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic. This parameter is optional.</param>
        /// <exception cref="ArgumentNullException">Thrown if the execute parameter is null.</exception>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            // Ensure the execute delegate is not null.
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. This parameter is not used.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            // If no canExecute predicate is provided, default to true.
            return canExecute == null || canExecute();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. This parameter is not used.</param>
        public void Execute(object? parameter)
        {
            // Invoke the execute action.
            execute();
        }
    }
}
