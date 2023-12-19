# This collection of functions provides access to EIGENVALUES decomposition methods
#
# Author: Eduardo G C Amaral
# Last update: April 1, 2023
#
# Use at your own risk

#[PACKAGES]
#[EXCEL_LAMBDA] R.EIGENVALUES=LAMBDA(x,[symmetric], XLRFUNC("base::eigen", "$values", "x", x, "symmetric", symmetric))
#[EXCEL_LAMBDA] R.EIGENVECTORS=LAMBDA(x,[symmetric], XLRFUNC("base::eigen", "$vectors", "x", x, "symmetric", symmetric))