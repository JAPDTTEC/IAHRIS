using global::System;
using global::System.ComponentModel;
using global::System.Diagnostics;

namespace IAHRIS.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            public FormAcercaDe m_FormAcercaDe;

            public FormAcercaDe FormAcercaDe
            {
                [DebuggerHidden]
                get
                {
                    m_FormAcercaDe = Create__Instance__(m_FormAcercaDe);
                    return m_FormAcercaDe;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FormAcercaDe))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FormAcercaDe);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public FormBienvenida m_FormBienvenida;

            public FormBienvenida FormBienvenida
            {
                [DebuggerHidden]
                get
                {
                    m_FormBienvenida = Create__Instance__(m_FormBienvenida);
                    return m_FormBienvenida;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FormBienvenida))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FormBienvenida);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public FormInicial m_FormInicial;

            public FormInicial FormInicial
            {
                [DebuggerHidden]
                get
                {
                    m_FormInicial = Create__Instance__(m_FormInicial);
                    return m_FormInicial;
                }

                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_FormInicial))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_FormInicial);
                }
            }
        }
    }
}