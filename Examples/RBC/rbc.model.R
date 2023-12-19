# Generated on 2023-12-19 17:52:39 by gEcon ver. 1.2.0 (2019-09-08)
# http://gecon.r-forge.r-project.org/

# Model name: rbc

# info
info__ <- c("rbc", "C:/Users/Eduardo/Google Drive/Meus Projetos/xlRcode/GitHub/Examples/RBC/rbc.gcn", "2023-12-19 17:52:39", "false")

# index sets
index_sets__ <- list()

# variables
variables__ <- c("r",
                 "C",
                 "I",
                 "K_s",
                 "L_s",
                 "U",
                 "W",
                 "Y",
                 "Z")

variables_tex__ <- c("r",
                     "C",
                     "I",
                     "K^{\\mathrm{s}}",
                     "L^{\\mathrm{s}}",
                     "U",
                     "W",
                     "Y",
                     "Z")

# shocks
shocks__ <- c("epsilon_Z")

shocks_tex__ <- c("\\epsilon^{\\mathrm{Z}}")

# parameters
parameters__ <- c("alpha",
                  "beta",
                  "delta",
                  "eta",
                  "mu",
                  "phi")

parameters_tex__ <- c("\\alpha",
                     "\\beta",
                     "\\delta",
                     "\\eta",
                     "\\mu",
                     "\\phi")

# free parameters
parameters_free__ <- c("beta",
                       "delta",
                       "eta",
                       "mu",
                       "phi")

# free parameters' values
parameters_free_val__ <- c(0.99,
                           0.025,
                           2,
                           0.3,
                           0.95)

# equations
equations__ <- c("-r[] + alpha * Z[] * K_s[-1]^(-1 + alpha) * L_s[]^(1 - alpha) = 0",
                 "-W[] + Z[] * (1 - alpha) * K_s[-1]^alpha * L_s[]^(-alpha) = 0",
                 "-Y[] + Z[] * K_s[-1]^alpha * L_s[]^(1 - alpha) = 0",
                 "-Z[] + exp(epsilon_Z[] + phi * log(Z[-1])) = 0",
                 "beta * (mu * E[][r[1] * C[1]^(-1 + mu) * (1 - L_s[1])^(1 - mu) * (C[1]^mu * (1 - L_s[1])^(1 - mu))^(-eta)] + mu * (1 - delta) * E[][C[1]^(-1 + mu) * (1 - L_s[1])^(1 - mu) * (C[1]^mu * (1 - L_s[1])^(1 - mu))^(-eta)]) - mu * C[]^(-1 + mu) * (1 - L_s[])^(1 - mu) * (C[]^mu * (1 - L_s[])^(1 - mu))^(-eta) = 0",
                 "(-1 + mu) * C[]^mu * (1 - L_s[])^(-mu) * (C[]^mu * (1 - L_s[])^(1 - mu))^(-eta) + mu * W[] * C[]^(-1 + mu) * (1 - L_s[])^(1 - mu) * (C[]^mu * (1 - L_s[])^(1 - mu))^(-eta) = 0",
                 "-C[] - I[] + Y[] = 0",
                 "I[] - K_s[] + K_s[-1] * (1 - delta) = 0",
                 "U[] - beta * E[][U[1]] - (1 - eta)^-1 * (C[]^mu * (1 - L_s[])^(1 - mu))^(1 - eta) = 0")

# calibrating equations
calibr_equations__ <- c("-0.36 * Y[ss] + r[ss] * K_s[ss] = 0")

# variables / equations map
vareqmap__ <- sparseMatrix(i = c(1, 1, 1, 1, 2, 2, 2, 2, 3, 3,
                                 3, 3, 4, 5, 5, 5, 6, 6, 6, 7,
                                 7, 7, 8, 8, 9, 9, 9),
                           j = c(1, 4, 5, 9, 4, 5, 7, 9, 4, 5,
                                 8, 9, 9, 1, 2, 5, 2, 5, 7, 2,
                                 3, 8, 3, 4, 2, 5, 6),
                           x = c(2, 1, 2, 2, 1, 2, 2, 2, 1, 2,
                                 2, 2, 3, 4, 6, 6, 2, 2, 2, 2,
                                 2, 2, 2, 3, 2, 2, 6),
                           dims = c(9, 9))

# variables / calibrating equations map
varcalibreqmap__ <- sparseMatrix(i = c(1, 1, 1),
                                 j = c(1, 4, 8),
                                 x = rep(1, 3), dims = c(1, 9))

# calibrated parameters / equations map
calibrpareqmap__ <- sparseMatrix(i = c(1, 2, 3),
                                 j = c(1, 1, 1),
                                 x = rep(1, 3), dims = c(9, 1))

# calibrated parameters / calibrating equations map
calibrparcalibreqmap__ <- sparseMatrix(i = NULL, j = NULL, dims = c(1, 1))

# free parameters / equations map
freepareqmap__ <- sparseMatrix(i = c(4, 5, 5, 5, 5, 6, 6, 8, 9, 9,
                                     9),
                               j = c(5, 1, 2, 3, 4, 3, 4, 2, 1, 3,
                                     4),
                               x = rep(1, 11), dims = c(9, 5))

# free parameters / calibrating equations map
freeparcalibreqmap__ <- sparseMatrix(i = NULL, j = NULL, dims = c(1, 5))

# shocks / equations map
shockeqmap__ <- sparseMatrix(i = c(4),
                             j = c(1),
                             x = rep(1, 1), dims = c(9, 1))

# steady state equations
ss_eq__ <- function(v, pc, pf)
{
    r <- numeric(9)
    r[1] = -v[1] + pc[1] * v[9] * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1])
    r[2] = -v[7] + v[9] * (1 - pc[1]) * v[4]^pc[1] * v[5]^(-pc[1])
    r[3] = -v[8] + v[9] * v[4]^pc[1] * v[5]^(1 - pc[1])
    r[4] = -v[9] + exp(pf[5] * log(v[9]))
    r[5] = pf[1] * (pf[4] * v[1] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[4] * (1 - pf[2]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])) - pf[4] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    r[6] = (-1 + pf[4]) * v[2]^pf[4] * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[4] * v[7] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    r[7] = -v[2] - v[3] + v[8]
    r[8] = v[3] - v[4] + v[4] * (1 - pf[2])
    r[9] = v[6] - pf[1] * v[6] - (1 - pf[3])^-1 * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(1 - pf[3])

    return(r)
}

# calibrating equations
calibr_eq__ <- function(v, pc, pf)
{
    r <- numeric(1)
    r[1] = -0.36 * v[8] + v[1] * v[4]

    return(r)
}

# steady state and calibrating equations Jacobian
ss_calibr_eq_jacob__ <- function(v, pc, pf)
{
    r <- numeric(1)
    jac <- numeric(33)
    jac[1] = -1
    jac[2] = pc[1] * v[9] * (-1 + pc[1]) * v[4]^(-2 + pc[1]) * v[5]^(1 - pc[1])
    jac[3] = pc[1] * v[9] * (1 - pc[1]) * v[4]^(-1 + pc[1]) * v[5]^(-pc[1])
    jac[4] = pc[1] * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1])
    jac[5] = v[9] * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1]) + pc[1] * v[9] * log(v[4]) * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1]) - pc[1] * v[9] * log(v[5]) * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1])
    jac[6] = pc[1] * v[9] * (1 - pc[1]) * v[4]^(-1 + pc[1]) * v[5]^(-pc[1])
    jac[7] = -pc[1] * v[9] * (1 - pc[1]) * v[4]^pc[1] * v[5]^(-1 - pc[1])
    jac[8] = -1
    jac[9] = (1 - pc[1]) * v[4]^pc[1] * v[5]^(-pc[1])
    jac[10] = -v[9] * v[4]^pc[1] * v[5]^(-pc[1]) + v[9] * log(v[4]) * (1 - pc[1]) * v[4]^pc[1] * v[5]^(-pc[1]) - v[9] * log(v[5]) * (1 - pc[1]) * v[4]^pc[1] * v[5]^(-pc[1])
    jac[11] = pc[1] * v[9] * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1])
    jac[12] = v[9] * (1 - pc[1]) * v[4]^pc[1] * v[5]^(-pc[1])
    jac[13] = -1
    jac[14] = v[4]^pc[1] * v[5]^(1 - pc[1])
    jac[15] = v[9] * log(v[4]) * v[4]^pc[1] * v[5]^(1 - pc[1]) - v[9] * log(v[5]) * v[4]^pc[1] * v[5]^(1 - pc[1])
    jac[16] = -1 + pf[5] * v[9]^-1 * exp(pf[5] * log(v[9]))
    jac[17] = pf[1] * pf[4] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    jac[18] = pf[1] * (-pf[3] * pf[4]^2 * v[1] * (v[2]^(-1 + pf[4]))^2 * ((1 - v[5])^(1 - pf[4]))^2 * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) - pf[3] * pf[4]^2 * (1 - pf[2]) * (v[2]^(-1 + pf[4]))^2 * ((1 - v[5])^(1 - pf[4]))^2 * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) + pf[4] * v[1] * (-1 + pf[4]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[4] * (-1 + pf[4]) * (1 - pf[2]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])) + pf[3] * pf[4]^2 * (v[2]^(-1 + pf[4]))^2 * ((1 - v[5])^(1 - pf[4]))^2 * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) - pf[4] * (-1 + pf[4]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    jac[19] = pf[1] * (pf[4] * v[1] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[4] * (-1 + pf[4]) * (1 - pf[2]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4] * v[1] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(-pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) - pf[3] * pf[4] * (-1 + pf[4]) * (1 - pf[2]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(-pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])) - pf[4] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[3] * pf[4] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(-pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])
    jac[20] = pf[4] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4]^2 * v[7] * (v[2]^(-1 + pf[4]))^2 * ((1 - v[5])^(1 - pf[4]))^2 * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) + pf[4] * v[7] * (-1 + pf[4]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(-pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])
    jac[21] = -pf[3] * (-1 + pf[4])^2 * (v[2]^pf[4])^2 * ((1 - v[5])^(-pf[4]))^2 * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) + pf[4] * (-1 + pf[4]) * v[2]^pf[4] * (1 - v[5])^(-1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[4] * v[7] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4] * v[7] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(-pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])
    jac[22] = pf[4] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    jac[23] = -1
    jac[24] = -1
    jac[25] = 1
    jac[26] = 1
    jac[27] = -pf[2]
    jac[28] = -pf[4] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    jac[29] = -(-1 + pf[4]) * v[2]^pf[4] * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    jac[30] = 1 - pf[1]
    jac[31] = v[4]
    jac[32] = v[1]
    jac[33] = -0.36
    jacob <- sparseMatrix(i = c(1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                                3, 3, 3, 3, 3, 4, 5, 5, 5, 6,
                                6, 6, 7, 7, 7, 8, 8, 9, 9, 9,
                                10, 10, 10),
                          j = c(1, 4, 5, 9, 10, 4, 5, 7, 9, 10,
                                4, 5, 8, 9, 10, 9, 1, 2, 5, 2,
                                5, 7, 2, 3, 8, 3, 4, 2, 5, 6,
                                1, 4, 8),
                          x = jac, dims = c(10, 10))

    return(jacob)
}

# 1st order perturbation
pert1__ <- function(v, pc, pf)
{
    Atm1x <- numeric(5)
    Atm1x[1] = pc[1] * v[9] * (-1 + pc[1]) * v[4]^(-2 + pc[1]) * v[5]^(1 - pc[1])
    Atm1x[2] = pc[1] * v[9] * (1 - pc[1]) * v[4]^(-1 + pc[1]) * v[5]^(-pc[1])
    Atm1x[3] = pc[1] * v[9] * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1])
    Atm1x[4] = pf[5] * v[9]^-1 * exp(pf[5] * log(v[9]))
    Atm1x[5] = 1 - pf[2]
    Atm1 <- sparseMatrix(i = c(1, 2, 3, 4, 8),
                         j = c(4, 4, 4, 9, 4),
                         x = Atm1x, dims = c(9, 9))

    Atx <- numeric(23)
    Atx[1] = -1
    Atx[2] = pc[1] * v[9] * (1 - pc[1]) * v[4]^(-1 + pc[1]) * v[5]^(-pc[1])
    Atx[3] = pc[1] * v[4]^(-1 + pc[1]) * v[5]^(1 - pc[1])
    Atx[4] = -pc[1] * v[9] * (1 - pc[1]) * v[4]^pc[1] * v[5]^(-1 - pc[1])
    Atx[5] = -1
    Atx[6] = (1 - pc[1]) * v[4]^pc[1] * v[5]^(-pc[1])
    Atx[7] = v[9] * (1 - pc[1]) * v[4]^pc[1] * v[5]^(-pc[1])
    Atx[8] = -1
    Atx[9] = v[4]^pc[1] * v[5]^(1 - pc[1])
    Atx[10] = -1
    Atx[11] = pf[3] * pf[4]^2 * v[2]^(-2 + 2 * pf[4]) * (1 - v[5])^(2 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) - pf[4] * (-1 + pf[4]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    Atx[12] = -pf[4] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[3] * pf[4] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(1 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])
    Atx[13] = pf[4] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(1 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) - pf[3] * pf[4]^2 * v[7] * v[2]^(-2 + 2 * pf[4]) * (1 - v[5])^(2 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) + pf[4] * v[7] * (-1 + pf[4]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    Atx[14] = -pf[3] * (-1 + pf[4])^2 * v[2]^(2 * pf[4]) * (1 - v[5])^(-2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3]) + pf[4] * (-1 + pf[4]) * v[2]^pf[4] * (1 - v[5])^(-1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) + pf[4] * v[7] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4] * v[7] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(1 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])
    Atx[15] = pf[4] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    Atx[16] = -1
    Atx[17] = -1
    Atx[18] = 1
    Atx[19] = 1
    Atx[20] = -1
    Atx[21] = -pf[4] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    Atx[22] = (1 - pf[4]) * v[2]^pf[4] * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    Atx[23] = 1
    At <- sparseMatrix(i = c(1, 1, 1, 2, 2, 2, 3, 3, 3, 4,
                             5, 5, 6, 6, 6, 7, 7, 7, 8, 8,
                             9, 9, 9),
                       j = c(1, 5, 9, 5, 7, 9, 5, 8, 9, 9,
                             2, 5, 2, 5, 7, 2, 3, 8, 3, 4,
                             2, 5, 6),
                         x = Atx, dims = c(9, 9))

    Atp1x <- numeric(4)
    Atp1x[1] = pf[1] * pf[4] * v[2]^(-1 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3])
    Atp1x[2] = pf[1] * (pf[4] * (v[1] * (-1 + pf[4]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4] * v[1] * v[2]^(-2 + 2 * pf[4]) * (1 - v[5])^(2 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])) + pf[4] * (1 - pf[2]) * ((-1 + pf[4]) * v[2]^(-2 + pf[4]) * (1 - v[5])^(1 - pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * pf[4] * v[2]^(-2 + 2 * pf[4]) * (1 - v[5])^(2 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])))
    Atp1x[3] = pf[1] * (pf[4] * (v[1] * (-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * v[1] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(1 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])) + pf[4] * (1 - pf[2]) * ((-1 + pf[4]) * v[2]^(-1 + pf[4]) * (1 - v[5])^(-pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-pf[3]) - pf[3] * (-1 + pf[4]) * v[2]^(-1 + 2 * pf[4]) * (1 - v[5])^(1 - 2 * pf[4]) * (v[2]^pf[4] * (1 - v[5])^(1 - pf[4]))^(-1 - pf[3])))
    Atp1x[4] = -pf[1]
    Atp1 <- sparseMatrix(i = c(5, 5, 5, 9),
                         j = c(1, 2, 5, 6),
                         x = Atp1x, dims = c(9, 9))

    Aepsx <- numeric(1)
    Aepsx[1] = exp(pf[5] * log(v[9]))
    Aeps <- sparseMatrix(i = c(4),
                         j = c(1),
                         x = Aepsx, dims = c(9, 1))

    return(list(Atm1, At, Atp1, Aeps))
}

ext__ <- list()

# create model object
gecon_model(model_info = info__,
            index_sets = index_sets__,
            variables = variables__,
            variables_tex = variables_tex__,
            shocks = shocks__,
            shocks_tex = shocks_tex__,
            parameters = parameters__,
            parameters_tex = parameters_tex__,
            parameters_free = parameters_free__,
            parameters_free_val = parameters_free_val__,
            equations = equations__,
            calibr_equations = calibr_equations__,
            var_eq_map = vareqmap__,
            shock_eq_map = shockeqmap__,
            var_ceq_map = varcalibreqmap__,
            cpar_eq_map = calibrpareqmap__,
            cpar_ceq_map = calibrparcalibreqmap__,
            fpar_eq_map = freepareqmap__,
            fpar_ceq_map = freeparcalibreqmap__,
            ss_function = ss_eq__,
            calibr_function = calibr_eq__,
            ss_calibr_jac_function = ss_calibr_eq_jacob__,
            pert = pert1__,
            ext = ext__)
